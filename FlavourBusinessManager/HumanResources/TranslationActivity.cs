using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Linq;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{8c7ce769-6c12-4aa6-92b7-3e1d6d239633}</MetaDataID>
    [BackwardCompatibilityID("{8c7ce769-6c12-4aa6-92b7-3e1d6d239633}")]
    [Persistent()]
    public class TranslationActivity : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.HumanResources.ITranslationActivity
    {


        ///// <exclude>Excluded</exclude>
        //System.Collections.Generic.List<TranslationSubject> _Subjects;

        ///// <MetaDataID>{39212040-d47c-40b4-9674-0865260ebe2a}</MetaDataID>
        //[Association("", Roles.RoleA, "260a3364-4645-4ab1-b417-d6db28a6da0f")]
        //[RoleAMultiplicityRange(1)]
        //public System.Collections.Generic.IList<TranslationSubject> Subjects
        //{
        //    get
        //    {
        //        if (_Subjects == null)
        //            _Subjects = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<TranslationSubject>>(SubjectsJson);

        //        return _Subjects.ToList();
        //    }
        //}

        ///// <MetaDataID>{caf32d13-b0cb-4c72-be1a-ed15e72320d6}</MetaDataID>
        //public TranslationSubject NewTranslationSubject(TranslationSubject.Type subjectType, string subjectIdentity)
        //{
        //    lock (SubjectsLock)
        //    {

        //        if (_Subjects == null)
        //            _Subjects = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<TranslationSubject>>(SubjectsJson);

        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {
        //            TranslationSubject translationSubject = new TranslationSubject() { SubjectType = subjectType, SubjcectIdentity = subjectIdentity };
        //            _Subjects.Add(translationSubject);
        //            SubjectsJson = OOAdvantech.Json.JsonConvert.SerializeObject(_Subjects);

        //            stateTransition.Consistent = true;
        //            return translationSubject;
        //        }
        //    }
        //}

        //object SubjectsLock = new object();

        //public void RemoveTranslationSubject(TranslationSubject subject)
        //{

        //    lock (SubjectsLock)
        //    {
        //        if (_Subjects == null)
        //            _Subjects = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<TranslationSubject>>(SubjectsJson);

        //        subject = _Subjects.Where(x => x.SubjcectIdentity == subject.SubjcectIdentity).FirstOrDefault();
        //        if (subject != null)
        //        {

        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Subjects.Remove(subject);
        //                SubjectsJson = OOAdvantech.Json.JsonConvert.SerializeObject(_Subjects);
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }

        //}

        ///// <MetaDataID>{39c7801e-6c95-4d29-b3c1-7404c856307b}</MetaDataID>
        //[PersistentMember]
        //[BackwardCompatibilityID("+2")]
        //string SubjectsJson = "[]";


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{a7fa5d0b-c78a-4d9b-a70f-457aa6fa5ff4}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        TranslationType _TranslationType;

        /// <MetaDataID>{8bcfb71c-68f8-49ed-8fd7-e0765e17c069}</MetaDataID>
        [PersistentMember(nameof(_TranslationType))]
        [BackwardCompatibilityID("+3")]
        public FlavourBusinessFacade.HumanResources.TranslationType TranslationType
        {
            get => _TranslationType;
            set
            {
                if (_TranslationType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TranslationType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _TranslationIdentity;

        /// <MetaDataID>{9604ea3a-1148-4f82-afd8-f228243824fc}</MetaDataID>
        [PersistentMember(nameof(_TranslationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string TranslationIdentity
        {
            get => _TranslationIdentity;

            set
            {

                if (_TranslationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TranslationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IAccountability> _Accountability = new OOAdvantech.Member<IAccountability>();
        /// <MetaDataID>{16bbee86-10e5-4c0c-8f23-5232365cb93c}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        public IAccountability Accountability => _Accountability.Value;
    }
}