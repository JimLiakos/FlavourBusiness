using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{e8b53bcd-b127-44bb-bc7b-4ae17c869e88}</MetaDataID>
    [BackwardCompatibilityID("{e8b53bcd-b127-44bb-bc7b-4ae17c869e88}")]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IMessageConsumer
    {
        [Association("ClientMessages", Roles.RoleA, "83213b62-9c18-4fb1-a8ab-2cbacb978f6e")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(0, 1)]
        System.Collections.Generic.IList<Message> Messages { get; }

        /// <MetaDataID>{508b502b-3425-49ab-b804-b763dc4d146b}</MetaDataID>
        void RemoveMessage(string messageId);


        /// <MetaDataID>{bd3f12bb-6062-4ac7-a604-23c1b39dbf8b}</MetaDataID>
        Message GetMessage(string messageId);

        /// <MetaDataID>{96fc92df-84bf-4bd1-b65a-15eedb2823ce}</MetaDataID>
        Message PeekMessage();

        /// <MetaDataID>{fcf4e4ae-183d-4884-b7b2-cba48a2f1ce1}</MetaDataID>
        Message PopMessage();

        /// <MetaDataID>{52d14444-cef0-480b-9f35-d02e5ce8b1d2}</MetaDataID>
        void PushMessage(Message message);

        event MessageReceivedHandle MessageReceived;
    }

    public delegate void MessageReceivedHandle(IMessageConsumer sender);

}  