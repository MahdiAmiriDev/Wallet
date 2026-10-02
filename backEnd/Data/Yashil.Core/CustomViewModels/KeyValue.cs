namespace Yashil.Core.CustomViewModels
{
    public class KeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
    public class SimpleModel
    {
        public SimpleModel(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}
