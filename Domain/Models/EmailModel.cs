namespace ProductAPI.Domain.Models {
    public class EmailModel {

        public string EmailTo { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }

        public string Subject { get; set; }
        
        public EmailModel (string emailTo, string body, string subject,string name)
        {
            EmailTo = emailTo;
            Name = name;
            Body = body;
            Subject = subject;
        }
        
        public EmailModel(){}
    
    }
}