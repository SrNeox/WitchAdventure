using System;
using System.Collections.Generic;
using _Project.Scripts.GoogleSheets;
using UnityEngine;
using NorskaLib.Spreadsheets;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "SpreadSheetsContainer", menuName = "Data/Data Container")]
    public class SpreadSheetsContainerAsset : SpreadsheetsContainerBase
    {
        [SpreadsheetContent] 
        [SerializeField] private SpreadSheetsContent _content;
        public SpreadSheetsContent Content => _content;
    }
}