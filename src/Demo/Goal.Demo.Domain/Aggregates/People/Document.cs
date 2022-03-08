using System;
using System.Diagnostics;
using Goal.Domain.Seedwork.Aggregates;

namespace Goal.Demo.Domain.Aggregates.People
{
    [DebuggerDisplay("Type = {Type}; Number = {Number}")]
    public class Document : Entity<string>
    {
        public DocumentType Type { get; private set; }
        public string Number { get; private set; }

        public Person Person { get; set; }

        protected Document()
            : base()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Document(DocumentType type, string number)
        {
            Type = type;
            Number = number;
        }

        public static Document CreateCpf(string number) => new(DocumentType.Cpf, number);
    }
}
