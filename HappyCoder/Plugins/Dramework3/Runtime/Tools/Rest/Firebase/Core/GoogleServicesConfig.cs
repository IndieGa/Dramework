using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core
{
    [Serializable]
    public class GoogleServicesConfig
    {
        #region ================================ FIELDS

        public ProjectInfo project_info;
        public Client[] client;
        public string configuration_version;

        #endregion

        #region ================================ NESTED TYPES

        [Serializable]
        public class Client
        {
            #region ================================ FIELDS

            public ClientInfo client_info;
            public OauthClient[] oauth_client;
            public ApiKey[] api_key;
            public Services services;

            #endregion

            #region ================================ NESTED TYPES

            [Serializable]
            public class ApiKey
            {
                #region ================================ FIELDS

                public string current_key;

                #endregion
            }

            [Serializable]
            public class ClientInfo
            {
                #region ================================ FIELDS

                public string mobilesdk_app_id;
                public AndroidClientInfo android_client_info;

                #endregion

                #region ================================ NESTED TYPES

                [Serializable]
                public class AndroidClientInfo
                {
                    #region ================================ FIELDS

                    public string package_name;

                    #endregion
                }

                #endregion
            }

            [Serializable]
            public class OauthClient
            {
                #region ================================ FIELDS

                public string client_id;
                public int client_type;

                #endregion
            }

            [Serializable]
            public class Services
            {
                #region ================================ FIELDS

                public AppinviteService appinvite_service;

                #endregion

                #region ================================ NESTED TYPES

                [Serializable]
                public class AppinviteService
                {
                    #region ================================ FIELDS

                    public OtherPlatformOauthClient[] other_platform_oauth_client;

                    #endregion

                    #region ================================ NESTED TYPES

                    [Serializable]
                    public class OtherPlatformOauthClient
                    {
                        #region ================================ FIELDS

                        public string client_id;
                        public int client_type;

                        #endregion
                    }

                    #endregion
                }

                #endregion
            }

            #endregion
        }

        [Serializable]
        public class ProjectInfo
        {
            #region ================================ FIELDS

            public string project_number;
            public string firebase_url;
            public string project_id;
            public string storage_bucket;

            #endregion
        }

        #endregion
    }
}