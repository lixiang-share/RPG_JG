package jg.rpg.exceptions;

public class PlayerHandlerException extends Exception {

	private static final long serialVersionUID = 1L;

	public PlayerHandlerException() {
		super();
	}

	public PlayerHandlerException(String message, Throwable cause, boolean enableSuppression, boolean writableStackTrace) {
		super(message, cause, enableSuppression, writableStackTrace);
	}

	public PlayerHandlerException(String message, Throwable cause) {
		super(message, cause);
	}

	public PlayerHandlerException(String message) {
		super(message);
	}

	public PlayerHandlerException(Throwable cause) {
		super(cause);
	}
}
