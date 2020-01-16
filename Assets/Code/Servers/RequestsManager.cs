﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using SoftLiu.Utilities;

namespace SoftLiu.Servers
{
    public class RequestsManager : AutoGeneratedSingleton<RequestsManager>
    {
        private int m_timeOut = 10;

        private List<RequestHandle> m_requestList = new List<RequestHandle>();

        public RequestsManager()
        {
            m_requestList.Clear();
        }

        public void Update()
        {
            for (int i = m_requestList.Count - 1; i >= 0; i--)
            {
                if (m_requestList[i].ProcessIfFinished())
                {
                    m_requestList.RemoveAt(i);
                }
            }
        }

        ~RequestsManager()
        {
            m_requestList.Clear();
        }

        public void GetServerTime(Action<GetServerTimeResponse> onGetServerTime)
        {
            string serverTimeUrl = "https://www.baidu.com/";
            UnityWebRequest request = UnityWebRequest.Get(serverTimeUrl);
            Action<string, string> onGetServerTimeResponseInternal = (string errorStr, string response) =>
            {
                bool result = true;
                if (string.IsNullOrEmpty(errorStr))
                {
                    Dictionary<string, string> headers = request.GetResponseHeaders();
                    if (headers.ContainsKey("Date"))
                    {
                        string time = headers["Date"];
                        result = false;
                        onGetServerTime(new GetServerTimeResponse(time, result));
                    }
                }
                if (result)
                {
                    onGetServerTime(new GetServerTimeResponse(errorStr, true));
                }
            };
            m_requestList.Add(new RequestHandle(request, onGetServerTimeResponseInternal));
            request.SendWebRequest();
        }

        public UnityWebRequest CreatePostRequet(string url, Dictionary<string, object> heade = null, Dictionary<string, object> parameters = null)
        {
            string fullUrl = url;
            WWWForm form = new WWWForm();
            if (parameters != null)
            {
                var iterator = parameters.GetEnumerator();
                {
                    while (iterator.MoveNext())
                    {
                        if (iterator.Current.Value != null)
                        {
                            form.AddField(iterator.Current.Key, iterator.Current.Value.ToString());
                        }
                    }
                }
            }
            UnityWebRequest request = UnityWebRequest.Post(fullUrl, form);
            request.timeout = m_timeOut;
            if (heade != null)
            {
                var iterator = heade.GetEnumerator();
                {
                    while (iterator.MoveNext())
                    {
                        if (iterator.Current.Value != null)
                        {
                            request.SetRequestHeader(iterator.Current.Key, iterator.Current.Value.ToString());
                        }
                    }
                }
            }
            return request;
        }
    }
}