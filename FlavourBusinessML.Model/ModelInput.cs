// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML.Data;

namespace FlavourBusinessML.Model
{
    public class ModelInput
    {
        [ColumnName("ID"), LoadColumn(0)]
        public float ID { get; set; }


        [ColumnName("OnPreparationItemsResourceUnits"), LoadColumn(1)]
        public float OnPreparationItemsResourceUnits { get; set; }


        [ColumnName("AvgTimeToComplete"), LoadColumn(2)]
        public float AvgTimeToComplete { get; set; }


        [ColumnName("PendingItemsResourceUnits"), LoadColumn(3)]
        public float PendingItemsResourceUnits { get; set; }


        [ColumnName("ItemResourceUnit"), LoadColumn(4)]
        public float ItemResourceUnit { get; set; }


        [ColumnName("DefaultItemPreparationTime"), LoadColumn(5)]
        public float DefaultItemPreparationTime { get; set; }


        [ColumnName("ItemName"), LoadColumn(6)]
        public string ItemName { get; set; }


        [ColumnName("ItemPreparationTime"), LoadColumn(7)]
        public float ItemPreparationTime { get; set; }


        [ColumnName("InputTime"), LoadColumn(8)]
        public string InputTime { get; set; }


        [ColumnName("Amount"), LoadColumn(9)]
        public float Amount { get; set; }


    }
}