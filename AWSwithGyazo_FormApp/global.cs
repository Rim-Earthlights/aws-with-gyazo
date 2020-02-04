namespace AWSwithGyazo_FormApp
{
    class global
    {
        //copy, erase, sync command
        internal static readonly string AWS_cp            = "aws s3 cp %SOURCE% s3://%UPPATH% --acl public-read";
        internal static readonly string AWS_cp_local      = "aws s3 cp s3://%UPPATH% %SOURCE%";
        internal static readonly string AWS_rm            = "aws s3 rm s3://%UPPATH%/%SOURCE%";
        internal static readonly string AWS_Sync_Local    = "aws s3 sync %SOURCE% s3://%UPPATH% --acl public-read";
        internal static readonly string AWS_Sync_S3       = "aws s3 sync s3://%UPPATH% %SOURCE% --acl public-read";
        internal static readonly string AWS_ls            = "aws s3 ls s3://%UPPATH%";
        internal static readonly string AWS_Add_Read_Cli  = "aws s3api put-object-acl --bucket %BUCKET% --key %FILENAME% --acl public-read";

        // load from setting.ini
        internal static string watchPath = "";
        internal static int SyncTime = 0;
        internal static readonly string settingFile = "setting.ini";
        internal static string awsId = null;
        internal static string awsKey = null;
        internal static string backupPath = "";


        // FileSystemWatcher 
        internal static System.IO.FileSystemWatcher watcher;

        // Watcher Status
        internal static bool enable_watch;

        // state auto backup
        internal static bool enable_backup;

        // Save copied File Path
        // internal static string beforePath = "";
    }
}
