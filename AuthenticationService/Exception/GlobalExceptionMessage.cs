namespace UserService.Exception
{
    public static class GlobalExceptionMessage
    {
        public static readonly string USER_LOCKED_MESSAGE = "The given user has been locked";
        public static readonly string USER_PASSWORD_ERROR = "The given password is wrong";
        public static readonly string USER_NAME_NOT_EXIST= "The given username is not exist";
        public static readonly string USER_CREATE_ERROR = "Create user error";
        public static readonly string USER_LOCK_SELF_ERROR = "You can not lock yourself";
        public static readonly string USER_HAS_EXIST = "The given user has exist";

        public static readonly string ROLE_HAS_EXIST = "The given role has exist";
        public static readonly string INVALID_ROLE_NAME = "The given role is not a valid name";
    }
}
