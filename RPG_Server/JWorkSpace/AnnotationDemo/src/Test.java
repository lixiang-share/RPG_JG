import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;


public class Test {

	public static void main(String[] args) throws IllegalAccessException, IllegalArgumentException, InvocationTargetException {
		// TODO Auto-generated method stub
		Fruit f = new Fruit();
		Field[] fs = f.getClass().getDeclaredFields();
		for(Field field : fs){
			System.out.println("=========");
			System.out.println(field.getAnnotation(TestAn.class).value());
			//System.out.println(field.getClass().getAnnotation(TestAn.class).value());
			if(fs.getClass().isAnnotationPresent(TestAn.class)){
				System.out.println("sdfsdf");
				System.out.println(f.getClass().getAnnotation(TestAn.class).value());
			}
		}
		
		MTest m = new MTest();
		Method[] ms = m.getClass().getMethods();
		for(Method md : ms){
			System.out.println(md.getName());
			if(md.isAnnotationPresent(TestMAn.class)){
				System.out.println("--------------");
				md.invoke(m, "hahahha");
			}
		}
		
		
		
		
		
		
	}

}
