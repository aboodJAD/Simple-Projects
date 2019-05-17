package ModelClasses;
import java.util.*;

public class GameCollector {
    ArrayList<EndedGame > savedGames;
    public GameCollector(){
        savedGames=new ArrayList<EndedGame>();
    }
    public void addGame(Game g,String x,String o){
        savedGames.add(new EndedGame(g,x,o));
    }
    public ArrayList<EndedGame> getAllGames(){
        return savedGames;
    }
/*    @Override
    public String toString(){
        String res="";
        for(EndedGame g:savedGames){
            res+=g.toString();
            res+="<br>";
        }
        if(res.isEmpty())res="<p>There are no previous games on this session</p>";
        return res;
    } */
}
