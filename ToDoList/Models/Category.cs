using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Category
    {
        public Category()
        {
            this.Items = new HashSet<CategoryItem>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CategoryItem> Items { get; set; }
        //This is a collection navigation property(CategoryItem) because it contains a reference to many related Items. If it was just a reference to a single entity(one-to-many relationship), it would be called a reference navigation property. 
    }
}