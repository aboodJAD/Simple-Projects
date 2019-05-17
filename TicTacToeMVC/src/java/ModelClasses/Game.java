package ModelClasses;

import java.util.*;
import java.util.Map.*;
import javafx.util.Pair;

public class Game {
    Cell [][] table;
    Value turn,winState;
    int cntEmp;
    int n,reqToWin;
    final int[] dirX={1,1,0,-1};
    final int[] dirY={0,1,1,1};
    public Game(int n,int reqToWin){
        this.n=n;
        this.reqToWin=reqToWin;
        table=new Cell[n][];
        for(int i=0;i<n;i++){
            table[i]=new Cell[n];
            for(int j=0;j<n;j++){
                table[i][j]=new Cell(i,j,Value.EMP);
            }
        }
        cntEmp=n*n;
        turn=Value.X;
        winState=Value.EMP;
    }
    public int getSize(){
        return n;
    }
    public String getCellName(int i,int j){
        return table[i][j].getName();
    }
    public String getCellValue(int i,int j){
        if(table[i][j].getValue()==Value.EMP)return "";
        if(table[i][j].getValue()==Value.X)return "X";
        return "O";
    }
    public boolean isStrongCell(int i,int j){
        return table[i][j].isStrong();
    }
    public void updateGame(Map<String,String> upd){
        int filledCells=0;
        int r=-1,c=-1;
        for(Entry<String,String> entry:upd.entrySet()){
            Pair<Integer,Integer > loc=Cell.ParseName(entry.getKey());
            Value v;
            try{
                v=Value.getValue(entry.getValue());
                if(table[loc.getKey()][loc.getValue()].getValue()==Value.EMP){
                    if(v==Value.EMP)continue;
                    filledCells++;
                    if(v!=turn||filledCells>1)throw new IllegalArgumentException("");
                    r=loc.getKey();
                    c=loc.getValue();
                }
            }catch(IllegalArgumentException ill){
                String message=ill.getMessage()+
                        ", Please keep the order of playing correct, next move is for ";
                if(turn==Value.X)message+="X player";
                else message+="O plyaer";
                throw new IllegalArgumentException(message);
            }
        }
        if(r==-1)return ;
        table[r][c].setValue(turn);
        if(turn==Value.X)turn=Value.O;
        else turn=Value.X;
        cntEmp--;
    }
    public Value getWinState(){
        if(n*n-cntEmp<5)return winState;
        checkWinState();
        return winState;
    }
    private void checkWinState(){
        for(int i=0;i<n;i++){
            for(int j=0;j<n;j++){
                for(int k=0;k<4;k++){
                    int cnt=0;
                    int r=i,c=j;
                    for(int p=0;p<reqToWin&&r<n&&c<n&&r>-1&&c>-1;p++){
                        if(table[r][c].getValue()==Value.EMP
                           ||table[r][c].getValue()!=table[i][j].getValue())break;
                        cnt++;
                        r+=dirY[k];
                        c+=dirX[k];
                    }
                    if(cnt==reqToWin){
                        r=i;
                        c=j;
                        for(int p=0;p<reqToWin&&r<n&&c<n&&r>-1&&c>-1;p++){
                            table[r][c].setStrong();
                            r+=dirY[k];
                            c+=dirX[k];
                        }
                        winState=table[i][j].getValue();
                        return ;
                    }
                }
            }
        }
    }
    public boolean isEnd(){
        checkWinState();
        return cntEmp==0||winState!=Value.EMP;
    }
   /* public String drawTable(){
        String res="";
        res+="<table border=\"1\" width=\"20%\" height=\"30%\">";
        for(int i=0;i<n;i++){
            res+="<tr>";
            for(int j=0;j<n;j++){
                char at;
                if(table[i][j].getValue()==Value.EMP)at=' ';
                else if(table[i][j].getValue()==Value.X)at='X';
                else at='O';
                res+="<td><input type=\"text\" name=\"";
                res+=table[i][j].getName();
                res+="\"";
                res+=" ";
                res+="value=";
                res+="\"";
                if(at!=' ')res+=at;
                res+="\"";
                res+=" ";
                res+="size=\"1\" maxlength=\"1\"";
                if(at!=' ')res+=" readonly";
                res+=" style=\"text-align:center;"
                     +" width:100%; height:100%;";
                if(table[i][j].isStrong())res+=" font-weight:bold";
                res+="\"";
                res+="></td>";
            }
            res+="</tr>";
        }
        res+="</table>";
        return res;
    }
    @Override
    public String toString(){
        return drawTable();
    }*/
}
