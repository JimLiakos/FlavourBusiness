namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{8bd0683a-8b95-41be-bd51-bbaf52832088}</MetaDataID>
    public interface ILayoutStyle : IStyleRule
    {
        /// <MetaDataID>{3fe15b85-ad6c-47ac-9c7a-30c4f8a6a455}</MetaDataID>
        double FontSizeScale { get; set; }

        /// <MetaDataID>{3b3138eb-bc73-4b8f-80bc-8a794d51edd7}</MetaDataID>
        string SeparationLineColor { get; set; }

        /// <MetaDataID>{e1c3ad4e-5882-41e4-b690-1011f414537b}</MetaDataID>
        double SeparationLineThickness { get; set; }

        /// <MetaDataID>{105d9a3c-d8a4-4faa-a360-2a8beca4b262}</MetaDataID>
        double LineSpacing { set; get; }
        /// <MetaDataID>{08f172f3-7245-4227-a1cc-37c5cf6a57ec}</MetaDataID>
        double NameIndent { set; get; }
        /// <MetaDataID>{bbf6939c-5131-4da4-8b41-d573dbb5857f}</MetaDataID>
        double DescLeftIndent { set; get; }
        /// <MetaDataID>{ea44ff63-1bf8-47ae-ba9d-9fca2959dc4f}</MetaDataID>
        double ExtrasLeftIndent { set; get; }
        /// <MetaDataID>{5742273e-3de4-48a3-9c03-787e98fc478a}</MetaDataID>
        double DescRightIndent { set; get; }
        /// <MetaDataID>{b5a71e94-81a3-4a09-8a65-ee95384549b6}</MetaDataID>
        string DescSeparator { set; get; }
        /// <MetaDataID>{40f82c9e-f94b-49d6-9930-b3b78fa34521}</MetaDataID>
        string ExtrasSeparator { set; get; }
        /// <MetaDataID>{05fa0bc0-0d4d-4b5e-9bae-9a8e0c49f18f}</MetaDataID>
        double SpaceBetweenColumns { set; get; }
        /// <MetaDataID>{84c34f7f-59e5-4dfa-9362-ac893aa0280e}</MetaDataID>
        bool LineBetweenColumns { set; get; }

        /// <MetaDataID>"{15e5e621-93cc-4626-9474-4c758130b8a1}"</MetaDataID>
        LineType SeparationLineType { set; get; }
    }

  
}


//<select name = "line-type" style="width:53px">
//				  			<option value = "single" selected="">|</option>
//				  			<option value = "double" >||</ option >
//                          <option value = "dashed">¦</option>
//				  			<option value = "dotted" >⋮</option> <!-- 11.0.58 -->
//				  			<option value = "arrowend2" >↕</option> <!-- 11.0.58 -->
//				  			<option value = "arrowend" >↕▴</option> 
//				  			<option value = "colbox" >⊡</option>
//							</select>