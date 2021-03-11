using System;

namespace AspNet.Dev.Pkg.Infrastructure.Dto
{
    public class UserInfoDto
    {
        public Guid? Id { get; set; }
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CardNo { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// stu teacher
        /// </summary>
        public string TypeCode { get; set; }
        public string SchoolNo { get; set; }
        public bool? IsSignUp { get; set; }
    }
}
