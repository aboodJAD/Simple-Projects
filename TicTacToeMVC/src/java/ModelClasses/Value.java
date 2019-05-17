package ModelClasses;

public enum Value {
    X,O,EMP;
    public static Value getValue(String val){
        if(val.isEmpty())return Value.EMP;
        if("x".equals(val.toLowerCase()))return Value.X;
        if("o".equals(val.toLowerCase()))return Value.O;
        throw new IllegalArgumentException("Wrong Input");
    }
}
