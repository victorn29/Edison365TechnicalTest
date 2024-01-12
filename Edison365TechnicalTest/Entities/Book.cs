namespace Edison365TechnicalTest.Entities
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<BookAuthor> BookAuthors { get; set; }
    }
}
