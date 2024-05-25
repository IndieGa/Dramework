using System.Collections.Generic;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Constants
{
    public static class HttpHelper
    {
        #region ================================ FIELDS

        private static readonly Dictionary<long, HttpStatus> Statutes = new Dictionary<long, HttpStatus>
        {
            // Unity responses
            { 0, new HttpStatus(0, HttpResponseClass.ClientError, "Cannot resolve destination host", "Cannot resolve destination host") },

            // Information responses
            { 100, new HttpStatus(100, HttpResponseClass.Informational, "Continue", "This interim response indicates that the client should continue the request or ignore the response if the request is already finished.") },
            { 101, new HttpStatus(101, HttpResponseClass.Informational, "Switching Protocols", "This code is sent in response to an Upgrade request header from the client and indicates the protocol the server is switching to.") },
            { 102, new HttpStatus(102, HttpResponseClass.Informational, "Processing", "This code indicates that the server has received and is processing the request, but no response is available yet.") },
            { 103, new HttpStatus(103, HttpResponseClass.Informational, "Early Hints", "This status code is primarily intended to be used with the Link header, letting the user agent start preloading resources while the server prepares a response.") },

            // Successful responses
            {
                200, new HttpStatus(200, HttpResponseClass.Successful, "OK", "The request succeeded. The result meaning of \"success\" depends on the HTTP method:" +
                                                                             "\n\nGET: The resource has been fetched and transmitted in the message body." +
                                                                             "\nHEAD: The representation headers are included in the response without any message body." +
                                                                             "\nPUT or POST: The resource describing the result of the action is transmitted in the message body." +
                                                                             "\nTRACE: The message body contains the request message as received by the server.")
            },
            { 201, new HttpStatus(201, HttpResponseClass.Successful, "Created", "The request succeeded, and a new resource was created as a result. This is typically the response sent after POST requests, or some PUT requests.") },
            { 202, new HttpStatus(202, HttpResponseClass.Successful, "Accepted", "The request has been received but not yet acted upon. It is noncommittal, since there is no way in HTTP to later send an asynchronous response indicating the outcome of the request. It is intended for cases where another process or server handles the request, or for batch processing.") },
            { 203, new HttpStatus(203, HttpResponseClass.Successful, "Non Authoritative Information", "This response code means the returned metadata is not exactly the same as is available from the origin server, but is collected from a local or a third-party copy. This is mostly used for mirrors or backups of another resource. Except for that specific case, the 200 OK response is preferred to this status.") },
            { 204, new HttpStatus(204, HttpResponseClass.Successful, "No Content", "There is no content to send for this request, but the headers may be useful. The user agent may update its cached headers for this resource with the new ones.") },
            { 205, new HttpStatus(205, HttpResponseClass.Successful, "Reset Content", "Tells the user agent to reset the document which sent this request.") },
            { 206, new HttpStatus(206, HttpResponseClass.Successful, "Partial Content", "This response code is used when the Range header is sent from the client to request only part of a resource.") },
            { 207, new HttpStatus(207, HttpResponseClass.Successful, "Multi Status", "Conveys information about multiple resources, for situations where multiple status codes might be appropriate.") },
            { 208, new HttpStatus(208, HttpResponseClass.Successful, "Already Reported", "Used inside a <dav:propstat> response element to avoid repeatedly enumerating the internal members of multiple bindings to the same collection.") },
            { 226, new HttpStatus(226, HttpResponseClass.Successful, "IM Used", "The server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.") },

            // Redirection messages
            { 300, new HttpStatus(300, HttpResponseClass.Redirection, "Multiple Choices", "The request has more than one possible response. The user agent or user should choose one of them. (There is no standardized way of choosing one of the responses, but HTML links to the possibilities are recommended so the user can pick.)") },
            { 301, new HttpStatus(301, HttpResponseClass.Redirection, "Moved Permanently", "The URL of the requested resource has been changed permanently. The new URL is given in the response.") },
            { 302, new HttpStatus(302, HttpResponseClass.Redirection, "Found", "This response code means that the URI of requested resource has been changed temporarily. Further changes in the URI might be made in the future. Therefore, this same URI should be used by the client in future requests.") },
            { 303, new HttpStatus(303, HttpResponseClass.Redirection, "See Other", "The server sent this response to direct the client to get the requested resource at another URI with a GET request.") },
            { 304, new HttpStatus(304, HttpResponseClass.Redirection, "Not Modified", "This is used for caching purposes. It tells the client that the response has not been modified, so the client can continue to use the same cached version of the response.") },
            { 305, new HttpStatus(305, HttpResponseClass.Redirection, "Use Proxy", "Defined in a previous version of the HTTP specification to indicate that a requested response must be accessed by a proxy. It has been deprecated due to security concerns regarding in-band configuration of a proxy.") },
            { 306, new HttpStatus(306, HttpResponseClass.Redirection, "Unused", "This response code is no longer used; it is just reserved. It was used in a previous version of the HTTP/1.1 specification.") },
            { 307, new HttpStatus(307, HttpResponseClass.Redirection, "Temporary Redirect", "The server sends this response to direct the client to get the requested resource at another URI with the same method that was used in the prior request. This has the same semantics as the 302 Found HTTP response code, with the exception that the user agent must not change the HTTP method used: if a POST was used in the first request, a POST must be used in the second request.") },
            { 308, new HttpStatus(308, HttpResponseClass.Redirection, "Permanent Redirect", "This means that the resource is now permanently located at another URI, specified by the Location: HTTP Response header. This has the same semantics as the 301 Moved Permanently HTTP response code, with the exception that the user agent must not change the HTTP method used: if a POST was used in the first request, a POST must be used in the second request.") },

            // Client error responses
            { 400, new HttpStatus(400, HttpResponseClass.ClientError, "Bad Request", "The server cannot or will not process the request due to something that is perceived to be a client error (e.g., malformed request syntax, invalid request message framing, or deceptive request routing).") },
            { 401, new HttpStatus(401, HttpResponseClass.ClientError, "Unauthorized", "Although the HTTP standard specifies \"unauthorized\", semantically this response means \"unauthenticated\". That is, the client must authenticate itself to get the requested response.") },
            { 402, new HttpStatus(402, HttpResponseClass.ClientError, "Payment Required", "This response code is reserved for future use. The initial aim for creating this code was using it for digital payment systems, however this status code is used very rarely and no standard convention exists.") },
            { 403, new HttpStatus(403, HttpResponseClass.ClientError, "Forbidden", "The client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401 Unauthorized, the client's identity is known to the server.") },
            { 404, new HttpStatus(404, HttpResponseClass.ClientError, "NotFound", "The server cannot find the requested resource. In the browser, this means the URL is not recognized. In an API, this can also mean that the endpoint is valid but the resource itself does not exist. Servers may also send this response instead of 403 Forbidden to hide the existence of a resource from an unauthorized client. This response code is probably the most well known due to its frequent occurrence on the web.") },
            { 405, new HttpStatus(405, HttpResponseClass.ClientError, "Method Not Allowed", "The request method is known by the server but is not supported by the target resource. For example, an API may not allow calling DELETE to remove a resource.") },
            { 406, new HttpStatus(406, HttpResponseClass.ClientError, "Not Acceptable", "This response is sent when the web server, after performing server-driven content negotiation, doesn't find any content that conforms to the criteria given by the user agent.") },
            { 407, new HttpStatus(407, HttpResponseClass.ClientError, "Proxy Authentication Required", "This is similar to 401 Unauthorized but authentication is needed to be done by a proxy.") },
            { 408, new HttpStatus(408, HttpResponseClass.ClientError, "Request Timeout", "This response is sent on an idle connection by some servers, even without any previous request by the client. It means that the server would like to shut down this unused connection. This response is used much more since some browsers, like Chrome, Firefox 27+, or IE9, use HTTP pre-connection mechanisms to speed up surfing. Also note that some servers merely shut down the connection without sending this message.") },
            { 409, new HttpStatus(409, HttpResponseClass.ClientError, "Conflict", "This response is sent when a request conflicts with the current state of the server.") },
            { 410, new HttpStatus(410, HttpResponseClass.ClientError, "Gone", "This response is sent when the requested content has been permanently deleted from server, with no forwarding address. Clients are expected to remove their caches and links to the resource. The HTTP specification intends this status code to be used for \"limited-time, promotional services\". APIs should not feel compelled to indicate resources that have been deleted with this status code.") },
            { 411, new HttpStatus(411, HttpResponseClass.ClientError, "Length Required", "Server rejected the request because the Content-Length header field is not defined and the server requires it.") },
            { 412, new HttpStatus(412, HttpResponseClass.ClientError, "Precondition Failed", "The client has indicated preconditions in its headers which the server does not meet.") },
            { 413, new HttpStatus(413, HttpResponseClass.ClientError, "Request Entity Too Large", "The client has indicated preconditions in its headers which the server does not meet.") },
            { 414, new HttpStatus(414, HttpResponseClass.ClientError, "Request Uri Too Long", "The URI requested by the client is longer than the server is willing to interpret.") },
            { 415, new HttpStatus(415, HttpResponseClass.ClientError, "Unsupported Media Type", "The media format of the requested data is not supported by the server, so the server is rejecting the request.") },
            { 416, new HttpStatus(416, HttpResponseClass.ClientError, "Requested Range Not Satisfiable", "The range specified by the Range header field in the request cannot be fulfilled. It's possible that the range is outside the size of the target URI's data.") },
            { 417, new HttpStatus(417, HttpResponseClass.ClientError, "Expectation Failed", "This response code means the expectation indicated by the Expect request header field cannot be met by the server.") },
            { 418, new HttpStatus(418, HttpResponseClass.ClientError, "I'm a Teapot", "The server refuses the attempt to brew coffee with a teapot.") },
            { 421, new HttpStatus(421, HttpResponseClass.ClientError, "Misdirected Request", "The request was directed at a server that is not able to produce a response. This can be sent by a server that is not configured to produce responses for the combination of scheme and authority that are included in the request URI.") },
            { 422, new HttpStatus(422, HttpResponseClass.ClientError, "Unprocessable Entity", "The request was well-formed but was unable to be followed due to semantic errors.") },
            { 423, new HttpStatus(423, HttpResponseClass.ClientError, "Locked", "The resource that is being accessed is locked.") },
            { 424, new HttpStatus(424, HttpResponseClass.ClientError, "Failed Dependency", "The request failed due to failure of a previous request.") },
            { 425, new HttpStatus(425, HttpResponseClass.ClientError, "To Early", "Indicates that the server is unwilling to risk processing a request that might be replayed.") },
            { 426, new HttpStatus(426, HttpResponseClass.ClientError, "Upgrade Required", "The server refuses to perform the request using the current protocol but might be willing to do so after the client upgrades to a different protocol. The server sends an Upgrade header in a 426 response to indicate the required protocol(s).") },
            { 428, new HttpStatus(428, HttpResponseClass.ClientError, "Precondition Required", "The origin server requires the request to be conditional. This response is intended to prevent the 'lost update' problem, where a client GETs a resource's state, modifies it and PUTs it back to the server, when meanwhile a third party has modified the state on the server, leading to a conflict.") },
            { 429, new HttpStatus(429, HttpResponseClass.ClientError, "Too Many Requests", "The user has sent too many requests in a given amount of time (\"rate limiting\").") },
            { 431, new HttpStatus(431, HttpResponseClass.ClientError, "Request Header Fields Too Large", "The server is unwilling to process the request because its header fields are too large. The request may be resubmitted after reducing the size of the request header fields.") },
            { 451, new HttpStatus(451, HttpResponseClass.ClientError, "Unavailable For Legal Reasons", "The user agent requested a resource that cannot legally be provided, such as a web page censored by a government.") },

            // Server error responses
            { 500, new HttpStatus(500, HttpResponseClass.ServerError, "Internal Server Error", "The server has encountered a situation it does not know how to handle.") },
            { 501, new HttpStatus(501, HttpResponseClass.ServerError, "Not Implemented", "The request method is not supported by the server and cannot be handled. The only methods that servers are required to support (and therefore that must not return this code) are GET and HEAD.") },
            { 502, new HttpStatus(502, HttpResponseClass.ServerError, "Bad Gateway", "This error response means that the server, while working as a gateway to get a response needed to handle the request, got an invalid response.") },
            { 503, new HttpStatus(503, HttpResponseClass.ServerError, "Service Unavailable", "The server is not ready to handle the request. Common causes are a server that is down for maintenance or that is overloaded. Note that together with this response, a user-friendly page explaining the problem should be sent. This response should be used for temporary conditions and the Retry-After HTTP header should, if possible, contain the estimated time before the recovery of the service. The webmaster must also take care about the caching-related headers that are sent along with this response, as these temporary condition responses should usually not be cached.") },
            { 504, new HttpStatus(504, HttpResponseClass.ServerError, "Gateway Timeout", "This error response is given when the server is acting as a gateway and cannot get a response in time.") },
            { 505, new HttpStatus(505, HttpResponseClass.ServerError, "Http Version Not Supported", "The HTTP version used in the request is not supported by the server.") },
            { 506, new HttpStatus(506, HttpResponseClass.ServerError, "Variant Also Negotiates", "The server has an internal configuration error: the chosen variant resource is configured to engage in transparent content negotiation itself, and is therefore not a proper end point in the negotiation process.") },
            { 507, new HttpStatus(507, HttpResponseClass.ServerError, "Insufficient Storage", "The method could not be performed on the resource because the server is unable to store the representation needed to successfully complete the request.") },
            { 508, new HttpStatus(508, HttpResponseClass.ServerError, "Loop Detected", "The server detected an infinite loop while processing the request.") },
            { 510, new HttpStatus(510, HttpResponseClass.ServerError, "Not Extended", "Further extensions to the request are required for the server to fulfill it.") },
            { 511, new HttpStatus(511, HttpResponseClass.ServerError, "Network Authentication Required", "Indicates that the client needs to authenticate to gain network access.") }
        };

        #endregion

        #region ================================ METHODS

        public static HttpStatus GetHttpStatus(long statusCode)
        {
            if (Statutes.TryGetValue(statusCode, out var status))
                return status;

            return new HttpStatus(statusCode, HttpResponseClass.UnknownCode, "UnknownCode", "UnknownCode");
        }

        #endregion
    }
}