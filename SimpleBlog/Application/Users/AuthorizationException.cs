﻿namespace SimpleBlog.Application.Users;

public class AuthorizationException : Exception
{
	public AuthorizationException(string message):base(message)
	{

	}
}