// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.Edc
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Nal.EncryptionModule
{
  public class Edc
  {
    private ushort edcCheck;
    private ushort edcCal;
    private Aes awStaticKey = new Aes();
    private Tdes tdwStaticKey = new Tdes();

    public Edc(string aesKey, string des1, string des2, string des3)
    {
      this.setStaticKeys(aesKey, des1, des2, des3);
    }

    public void setStaticKeys(string aesKey, string des1, string des2, string des3)
    {
      if (aesKey.Length != 32 || des1.Length != 8 || des2.Length != 8 || des3.Length != 8)
        return;
      this.tdwStaticKey.SetKeys(des1, des2, des3);
      this.awStaticKey.SetKey(aesKey);
    }

    public bool checkAndCalMatch() => (int) this.edcCheck == (int) this.edcCal;

    public bool setEdcCal(string fileName)
    {
      byte[] bytes;
      try
      {
        bytes = File.ReadAllBytes(fileName);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
      this.edcCal = this.crc16(bytes);
      return true;
    }

    public bool loadEdcFile(string in_edcf)
    {
      byte[] bytes;
      try
      {
        bytes = File.ReadAllBytes(in_edcf);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
      if (bytes.Length != 16)
        return false;
      byte[] sourceArray = this.tdwStaticKey.Decrypt(this.awStaticKey.Decrypt(bytes));
      if (sourceArray == null)
        return false;
      byte[] numArray = new byte[4];
      Array.Copy((Array) sourceArray, (Array) numArray, 4);
      if (Encoding.GetEncoding(1252).GetString(numArray) != "~Un5")
        return false;
      this.edcCheck = (ushort) ((uint) (ushort) sourceArray[4] * 256U + (uint) (ushort) sourceArray[5]);
      return true;
    }

    public bool generateEdcFile(string fileName)
    {
      string data = "~Un5" + ((char) ((uint) this.edcCal / 256U)).ToString() + ((char) ((uint) this.edcCal % 256U)).ToString();
      Utils.AppendTo16Block(ref data);
      byte[] bytes = this.awStaticKey.Encrypt(this.tdwStaticKey.Encrypt(Encoding.GetEncoding(1252).GetBytes(data)));
      try
      {
        File.WriteAllBytes(fileName, bytes);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
      return true;
    }

    private ushort crc16(byte[] bytes)
    {
      ushort[] numArray = new ushort[256];
      ushort maxValue = ushort.MaxValue;
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        ushort num1 = 0;
        ushort num2 = (ushort) (index1 << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((((int) num1 ^ (int) num2) & 32768) != 0)
            num1 = (ushort) ((int) num1 << 1 ^ 4129);
          else
            num1 <<= 1;
          num2 <<= 1;
        }
        numArray[index1] = num1;
      }
      ushort num = maxValue;
      int length = bytes.Length;
      for (int index = 0; index < length; ++index)
        num = (ushort) ((uint) num << 8 ^ (uint) numArray[(int) num >> 8 ^ (int) byte.MaxValue & (int) bytes[index]]);
      return num;
    }
  }
}
