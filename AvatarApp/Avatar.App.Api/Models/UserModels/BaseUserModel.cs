﻿using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public abstract class BaseUserModel
    {
        protected BaseUserModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            ProfilePhoto = user.ProfilePhoto;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
    }
}