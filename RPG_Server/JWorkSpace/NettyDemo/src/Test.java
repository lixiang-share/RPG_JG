
public class Test {

	
	
	
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		int port = 12345;
		try {
			new DiscardServer(port).run();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
