#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;


namespace IG.HappyCoder.Dramework3.Runtime.Google
{
    public class GoogleSheetsTable : IEnumerable<string[]>
    {
        #region ================================ FIELDS

        private readonly List<string[]> _table = new List<string[]>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GoogleSheetsTable(string input)
        {
            var lines = input.Split('\n');
            foreach (var line in lines)
                _table.Add(line.Split('\t'));
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public int Count => _table.Count;
        public string[] this[int r] => _table[r];
        public string this[int r, int c] => _table[r][c];

        #endregion

        #region ================================ METHODS

        public IEnumerator<string[]> GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
#endif