﻿namespace QuokkaMesh.Models.DTOModel
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
