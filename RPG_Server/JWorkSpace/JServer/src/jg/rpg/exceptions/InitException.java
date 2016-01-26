package jg.rpg.exceptions;

public class InitException extends Exception {

	private static final long serialVersionUID = 1L;

	public InitException() {
		super();
	}

	public InitException(String message, Throwable cause, boolean enableSuppression, boolean writableStackTrace) {
		super(message, cause, enableSuppression, writableStackTrace);
	}

	public InitException(String message, Throwable cause) {
		super(message, cause);
	}

	public InitException(String message) {
		super(message);
	}

	public InitException(Throwable cause) {
		super(cause);
	}
	
	

}
