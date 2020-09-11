using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AWSwithGyazo_FormApp {
    /// <summary>
    /// 認証情報設定クラス
    /// </summary>
    class CredentialManager {

        private string accessKey;
        private string secretKey;
        private RegionEndpoint regionEndpoint;

        /// <summary>
        /// 認証情報を設定します。
        /// </summary>
        /// <param name="accessKey">AWS_ID</param>
        /// <param name="secretKey">AWS_SECRET_KEY</param>
        /// <param name="endPoint">REGION_ENDPOINT</param>
        public CredentialManager(string accessKey, string secretKey, RegionEndpoint endPoint) {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.regionEndpoint = endPoint;
        }

        public IAmazonS3 CreateS3Client() {
            return new AmazonS3Client(accessKey, secretKey, regionEndpoint);
        }

    }
}
