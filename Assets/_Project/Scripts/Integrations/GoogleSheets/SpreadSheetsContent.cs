using System.Collections.Generic;
using NorskaLib.Spreadsheets;

namespace _Project.Scripts.GoogleSheets
{
    [System.Serializable]
    public class SpreadSheetsContent
    {
        [SpreadsheetPage("Units")]
        public List<UnitData> Units;
    }
}
