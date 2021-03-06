namespace UserService.Exception
{
    public static class GlobalExceptionCode
    {
        public static readonly int USER_LOCKED_CODE = 1001;
        public static readonly int USER_PASSWORD_WRONG_CODE = 1002;
        public static readonly int USER_NAME_NOT_EXIST_CODE = 1003;
        public static readonly int USER_CREATION_ERROR_CODE = 1004;
        public static readonly int USER_LOCK_SELF_CODE = 1005;
        public static readonly int USER_UNLOCK_SELF_CODE = 1006;
        public static readonly int USER_HAS_EXIST = 1007;
        public static readonly int USER_NOT_LOGIN = 1008;

        public static readonly int ROLE_HAS_EXIST = 2001;
        public static readonly int INVALID_ROLE_NAME = 2002;

        public static readonly int INVALID_AVATAR_FORMAT = 3001;
        public static readonly int AVATAR_NOT_EXIST = 3002;
    }
}
