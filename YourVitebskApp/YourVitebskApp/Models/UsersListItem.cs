﻿namespace YourVitebskApp.Models
{
    public class UsersListItem
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVisible { get; set; }
        public string Image { get; set; }
    }
}
