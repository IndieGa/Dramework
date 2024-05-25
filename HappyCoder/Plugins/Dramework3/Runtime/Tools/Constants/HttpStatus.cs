namespace IG.HappyCoder.Dramework3.Runtime.Tools.Constants
{
    public class HttpStatus
    {
        #region ================================ FIELDS

        public readonly long Code;
        public readonly HttpResponseClass ResponseClass;
        public readonly string Name;
        public readonly string Description;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public HttpStatus(long code, HttpResponseClass responseClass, string name, string description)
        {
            Code = code;
            ResponseClass = responseClass;
            Name = name;
            Description = description;
        }

        #endregion
    }
}