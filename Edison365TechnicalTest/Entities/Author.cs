namespace Edison365TechnicalTest.Entities
{
    public class Author
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<BookAuthor> BookAuthors { get; set; }
    }
}
