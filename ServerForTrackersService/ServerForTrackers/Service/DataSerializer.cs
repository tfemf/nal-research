// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Service.DataSerializer
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Nal.ServerForTrackers.Service
{
  public class DataSerializer
  {
    private DataSerializer.SendDataDelegate sendData;
    private CharProcessor charProcessor;
    private DataSerializer.Transaction inProcessTransReq;
    private List<DataSerializer.Transaction> waitingTransactions;

    public DataSerializer(DataSerializer.SendDataDelegate sendData)
    {
      this.sendData = sendData;
      this.charProcessor = new CharProcessor();
      this.charProcessor.LookupFinished += new CharProcessor.LookupFinishedEventHandler(this.OnCharProcessorLookupFinished);
      this.waitingTransactions = new List<DataSerializer.Transaction>();
    }

    public event DataSerializer.TransactionCompletedEventHandler TransactionCompleted;

    public void CancelInProcessTransaction()
    {
      if (this.charProcessor.Looking)
        this.charProcessor.CancelLookUp();
      this.StartNextTransaction();
    }

    public void CancelTransactions()
    {
      this.charProcessor.CancelLookUp();
      this.waitingTransactions.Clear();
    }

    public void CancelWaitingTransaction(string id)
    {
      for (int index = 0; index < this.waitingTransactions.Count; ++index)
      {
        if (this.waitingTransactions[index].ID == id)
        {
          this.waitingTransactions.RemoveAt(index);
          break;
        }
      }
    }

    public void CancelWaitingTransactions() => this.waitingTransactions.Clear();

    public string GetInProcessTransactionID()
    {
      return !this.TransactionInProgress() ? string.Empty : this.inProcessTransReq.ID;
    }

    public void ProcessReceivedData(string data) => this.charProcessor.HandleReceivedData(data);

    public bool TransactionInProgress() => this.charProcessor.Looking;

    public bool TransactionsWaiting() => this.waitingTransactions.Count > 0;

    public bool TransactionWaiting(string id)
    {
      foreach (DataSerializer.Transaction waitingTransaction in this.waitingTransactions)
      {
        if (waitingTransaction.ID == id)
          return true;
      }
      return false;
    }

    public void RequestTransaction(
      string dataToSend,
      string dataToReceive,
      int timeout,
      string id)
    {
      this.waitingTransactions.Add(new DataSerializer.Transaction(dataToSend, dataToReceive, timeout, id));
      if (this.charProcessor.Looking)
        return;
      this.StartNextTransaction();
    }

    private void OnCharProcessorLookupFinished(
      object sender,
      CharProcessor.LookupFinishedEventArgs e)
    {
      DataSerializer.Transaction inProcessTransReq = this.inProcessTransReq;
      this.StartNextTransaction();
      if (this.TransactionCompleted == null)
        return;
      this.TransactionCompleted((object) this, new DataSerializer.TransactionCompletedEventArgs(inProcessTransReq.ID, e.Result));
    }

    private void StartNextTransaction()
    {
      while (this.waitingTransactions.Count > 0 && this.waitingTransactions[0].DataToReceive == string.Empty)
      {
        this.sendData(Encoding.GetEncoding(1252).GetBytes(this.waitingTransactions[0].DataToSend));
        this.waitingTransactions.RemoveAt(0);
      }
      if (this.waitingTransactions.Count <= 0)
        return;
      this.inProcessTransReq = this.waitingTransactions[0];
      this.waitingTransactions.RemoveAt(0);
      if (this.inProcessTransReq.DataToSend != string.Empty)
        this.sendData(Encoding.GetEncoding(1252).GetBytes(this.inProcessTransReq.DataToSend));
      this.charProcessor.ClearBuffer();
      this.charProcessor.LookUp(this.inProcessTransReq.DataToReceive, this.inProcessTransReq.Timeout, false, this.inProcessTransReq.ID);
    }

    public delegate void SendDataDelegate(byte[] data);

    public delegate void TransactionCompletedEventHandler(
      object sender,
      DataSerializer.TransactionCompletedEventArgs e);

    private struct Transaction
    {
      public string DataToSend;
      public string DataToReceive;
      public int Timeout;
      public string ID;

      public Transaction(string dataToSend, string dataToReceive, int timeout, string id)
      {
        this.DataToSend = dataToSend;
        this.DataToReceive = dataToReceive;
        this.Timeout = timeout;
        this.ID = id;
      }
    }

    public class TransactionCompletedEventArgs : EventArgs
    {
      private string transactionID;
      private int result;

      public TransactionCompletedEventArgs(string transactionID, int result)
      {
        this.transactionID = transactionID;
        this.result = result;
      }

      public string TransactionID => this.transactionID;

      public int Result => this.result;
    }
  }
}
