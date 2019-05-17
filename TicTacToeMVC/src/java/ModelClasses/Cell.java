package ModelClasses;

import javafx.util.Pair;

public class Cell {
    int row,col;
    Value val;
    boolean strong;
    public Cell(int r,int c,Value v){
        val=v;
        row=r;
        col=c;
        strong=false;
    }
    public void  setValue(Value v){
        if(val==v||val==Value.EMP)val=v;
        else throw new IllegalArgumentException("");
    }
    public Value getValue(){return val;}
    public String getName(){return "Cell"+row+"_"+col;}
    public void setStrong(){strong=true;}
    public boolean isStrong(){return strong;}
    public static Pair<Integer,Integer> ParseName(String name){
        name=name.substring(4);
        String row=name.substring(0, name.indexOf('_'));
        String col=name.substring(name.indexOf('_')+1,name.length());
        return new Pair<>(Integer.parseInt(row),Integer.parseInt(col));
    }
}
