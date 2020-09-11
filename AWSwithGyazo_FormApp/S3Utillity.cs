using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AWSwithGyazo_FormApp {
    class S3Utillity {
        private IAmazonS3 client;

        public S3Utillity(IAmazonS3 client) {
            this.client = client;
        }

        public async Task UploadFileAsync(string filePath, string bucketName, string bucketPath) {
            var putRequest = new PutObjectRequest {
                BucketName = bucketName,
                Key = bucketPath,
                FilePath = filePath,
                ContentType = "text/plain"
            };

            PutObjectResponse response1 = await client.PutObjectAsync(putRequest);
            Console.WriteLine("Upload Complete");
        }
        public async Task ListObjectAsync(string bucketName) {
            ListObjectsRequest request = new ListObjectsRequest {
                BucketName = bucketName,
                MaxKeys = 2
            };
            do {
                ListObjectsResponse response = await client.ListObjectsAsync(request);
                // Process the response.
                foreach (S3Object entry in response.S3Objects) {
                    Console.WriteLine("key = {0} size = {1}",
                        entry.Key, entry.Size);
                }

                // If the response is truncated, set the marker to get the next 
                // set of keys.
                if (response.IsTruncated) {
                    request.Marker = response.NextMarker;
                } else {
                    request = null;
                }
            } while (request != null);
        }
    }
}
