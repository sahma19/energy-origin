using System;

namespace API.Models;

public class ClientCredential
{
   private ClientCredential(string hint, string keyId, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
   {
      Hint = hint;
      KeyId = keyId;
      StartDateTime = startDateTime;
      EndDateTime = endDateTime;
   }

   public string Hint { get; set; }
   public string KeyId { get; set; }
   public DateTimeOffset StartDateTime { get; set; }
   public DateTimeOffset EndDateTime { get; set; }

   public static ClientCredential Create(string hint, string keyId, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
   {
      return new ClientCredential(hint, keyId, startDateTime, endDateTime);
   }
}
