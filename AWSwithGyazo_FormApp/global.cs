namespace AWSwithGyazo_FormApp
{
    class global
    {
        //copy, erase, sync command
        internal static readonly string AWS_cp       = "aws s3 cp %SOURCE% s3://%UPPATH% --grants read=uri=http://acs.amazonaws.com/groups/global/AllUsers";
        internal static readonly string AWS_cp_local = "aws s3 cp s3://%UPPATH% %SOURCE%";
        internal static readonly string AWS_rm       = "aws s3 rm s3://%UPPATH%/%SOURCE%";
        // internal static readonly string AWS_Sync_toLocal = "aws s3 sync s3://%UPPATH% %SOURCE%";
        // internal static readonly string AWS_Sync_toAWS = "aws s3 sync %SOURCE% s3://%UPPATH% --grants read=uri=http://acs.amazonaws.com/groups/global/AllUsers";
        internal static readonly string AWS_ls       = "aws s3 ls s3://%UPPATH%";


        // load from setting.ini
        internal static string watchPath = "";
        internal static int SyncTime = 0;
        internal static readonly string settingFile = "setting.ini";
        internal static string awsId = null;
        internal static string awsKey = null;


        // FileSystemWatcher 
        internal static System.IO.FileSystemWatcher watcher;

        // Watcher Status
        internal static bool enable_watch;

        // Save copied File Path
        // internal static string beforePath = "";
    }
}
