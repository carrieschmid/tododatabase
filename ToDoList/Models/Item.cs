using System;
using System.Collections.Generic;

namespace ToDoList.Models {
  public class Item {
    public Item () {
      this.Categories = new HashSet<CategoryItem> ();
    }
    public int ItemId { get; set; }
    public string Description { get; set; }
    public virtual ApplicationUser User { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime Date { get; set; }
    public ICollection<CategoryItem> Categories { get; }
    //We could add a setter method to the Categories property as well but we'll only be modifying the relationship between an Item and a Category via a Category's Items in our application.
  }
}