﻿using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseEx;

namespace MenuPresentationModel
{
    /// <MetaDataID>{9e1dd6ca-1019-4f6d-8da5-413944f09a64}</MetaDataID>
    [BackwardCompatibilityID("{9e1dd6ca-1019-4f6d-8da5-413944f09a64}")]
    [Persistent()]
    public class ItemExtraInfoStyleSheet : ExtMarshalByRefObject, IItemExtraInfoStyleSheet
    {
    

        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        MultilingualMember<FontData> _HeadingFont = new MultilingualMember<FontData>();

        /// <MetaDataID>{89c6a493-f3d9-471a-9465-d9599b1ab631}</MetaDataID>
        [PersistentMember(nameof(_HeadingFont))]
        [BackwardCompatibilityID("+1")]
        public UIBaseEx.FontData HeadingFont
        {
            get => _HeadingFont; set
            {
                if (_HeadingFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HeadingFont.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<FontData> _ParagraphFont = new MultilingualMember<FontData>();

        /// <MetaDataID>{f20e80a7-2fee-459e-a5fa-a0fd35daf5f2}</MetaDataID>
        [PersistentMember(nameof(_ParagraphFont))]
        [BackwardCompatibilityID("+3")]
        public UIBaseEx.FontData ParagraphFont
        {
            get => _ParagraphFont.Value;
            set
            {
                if (_ParagraphFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ParagraphFont.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<FontData?> _ParagraphFirstLetterFont = new MultilingualMember<FontData?>();

        /// <MetaDataID>{9055d43d-2368-493a-b4f3-d4ff8ceccb61}</MetaDataID>
        [PersistentMember(nameof(_ParagraphFirstLetterFont))]
        [BackwardCompatibilityID("+2")]
        public UIBaseEx.FontData? ParagraphFirstLetterFont
        {
            get => _ParagraphFirstLetterFont;
            set
            {
                if (_ParagraphFirstLetterFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ParagraphFirstLetterFont.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{1c955bcd-5747-4521-8673-5c6172d92a61}</MetaDataID>
        public Multilingual MultilingualHeadingFont => new Multilingual(_HeadingFont);

        /// <MetaDataID>{578f6441-75ea-4dc1-a4b8-8560ea7ca213}</MetaDataID>
        public Multilingual MultilingualParagraphFont => new Multilingual(_ParagraphFont);

        /// <MetaDataID>{faf442c4-f7e2-49d5-8367-8674066049d7}</MetaDataID>
        public Multilingual MultilingualParagraphFirstLetterFont => new Multilingual(_ParagraphFirstLetterFont);


        /// <MetaDataID>{04ac7b4a-22af-4e4d-9ae3-770a5112f927}</MetaDataID>
        public Multilingual MultilingualItemInfoFirstLetterLeftIndent => new Multilingual(_ItemInfoFirstLetterLeftIndent);

        /// <MetaDataID>{0911e3c3-af8f-4bd5-b722-7a75223e1469}</MetaDataID>
        public Multilingual MultilingualItemInfoFirstLetterRightIndent => new Multilingual(_ItemInfoFirstLetterRightIndent);

        /// <MetaDataID>{ce27082c-1062-471f-982c-d31b0b03ad18}</MetaDataID>
        public Multilingual MultilingualItemInfoFirstLetterLinesSpan => new Multilingual(_ItemInfoFirstLetterLinesSpan);



        /// <exclude>Excluded</exclude>
        MultilingualMember<int?> _ItemInfoFirstLetterLeftIndent = new MultilingualMember<int?>();
        /// <MetaDataID>{41468368-8311-4aad-b284-970a90335d43}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterLeftIndent))]
        [BackwardCompatibilityID("+4")]
        public int? ItemInfoFirstLetterLeftIndent
        {
            get => _ItemInfoFirstLetterLeftIndent.Value;
            set
            {
                if (_ItemInfoFirstLetterLeftIndent.Value!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterLeftIndent.Value=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<int?> _ItemInfoFirstLetterRightIndent = new MultilingualMember<int?>();

        /// <MetaDataID>{9da0f9e2-0559-443d-a734-7fddaf4fce08}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterRightIndent))]
        [BackwardCompatibilityID("+5")]
        public int? ItemInfoFirstLetterRightIndent
        {
            get => _ItemInfoFirstLetterRightIndent.Value;
            set
            {
                if (_ItemInfoFirstLetterRightIndent.Value!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterRightIndent.Value=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<int?> _ItemInfoFirstLetterLinesSpan = new MultilingualMember<int?>();

        /// <MetaDataID>{9acd1b2e-597a-4561-8362-fe9804b72363}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterLinesSpan))]
        [BackwardCompatibilityID("+6")]
        public int? ItemInfoFirstLetterLinesSpan
        {
            get => _ItemInfoFirstLetterLinesSpan.Value;
            set
            {
                if (_ItemInfoFirstLetterLinesSpan.Value!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterLinesSpan.Value=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        ///// <MetaDataID>{9acd1b2e-597a-4561-8362-fe9804b72363}</MetaDataID>
        //public int? ItemInfoFirstLetterLinesSpan {
        //    get => _ItemInfoFirstLetterLinesSpan.Value;
        //    set => throw new NotImplementedException();
        //}

     
    }
}
