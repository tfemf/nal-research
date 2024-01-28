// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.Erc
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

#nullable disable
namespace Nal.EncryptionModule
{
  public enum Erc
  {
    Success,
    Exception,
    SoftwareIntegrityTestError,
    EncryptionTestError,
    SoftwareIntegrityEncryptionTestsError,
    UserFileLockedAndNoTemporary,
    CryptoOfficerFileLocked,
    AlreadyLoggedIn,
    NotLoggedIn,
    EncryptionKeyNotSetForModem,
    DecryptionKeyNotFound,
    DecryptionKeyNotSetForModem,
    ErrorInDecryption,
    ModemNotFound,
    RecordDoesNotExist,
    RecordAlreadyExists,
    FileDoesNotExist,
    InvalidEncryptionBlock,
    InvalidModem,
    InvalidUser,
    InvalidPassword,
    InvalidEncryptionKey,
    InvalidDecryptionKey,
    CouldNotSaveAllUserFiles,
    CouldNotCreateKeysDirectory,
    CouldNotCopyUserFile,
    CouldNotCreateFile,
    CouldNotReadFile,
    CouldNotWriteFile,
    CouldNotLockFile,
    CouldNotUnlockFile,
  }
}
