package jg.rpg.common.exceptions;

public class EntityHandlerException extends Exception {
	private static final long serialVersionUID = 1L;

	public EntityHandlerException() {
		super();
	}

	public EntityHandlerException(String message, Throwable cause, boolean enableSuppression, boolean writableStackTrace) {
		super(message, cause, enableSuppression, writableStackTrace);
	}

	public EntityHandlerException(String message, Throwable cause) {
		super(message, cause);
	}

	public EntityHandlerException(String message) {
		super(message);
	}

	public EntityHandlerException(Throwable cause) {
		super(cause);
	}
}
