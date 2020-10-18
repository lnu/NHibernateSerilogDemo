namespace NHibernateDemo.Model
{
    using System;

    public class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
    }
}
