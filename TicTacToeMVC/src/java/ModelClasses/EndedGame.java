package ModelClasses;

public class EndedGame {
    Game game;
    String xPlayer,oPlayer;
    public EndedGame(Game g,String x,String o){
        game=g;
        xPlayer=x;
        oPlayer=o;
    }
    public Game getGame(){return game;};
    public String getXPlayer(){return xPlayer;}
    public String getOPlayer(){return oPlayer;}
 /*   @Override
    public String toString(){
        String res="",xHeader="Player X : "+xPlayer,
                oHeader="Player O : "+oPlayer;
        if(game.getWinState()==Value.EMP){
            res+="Game ended with tie<br>";
        }else if(game.getWinState()==Value.X){
            xHeader="<strong>"+xHeader+" (Winner)</Strong>";
        }else {
            oHeader="<strong>"+oHeader+" (Winner)</Strong>";
        }
        res+="<p>"+xHeader+"&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp"
                +oHeader+"</p>";
        res+=game.toString();
        res+="<br>";
        return res;
    }    */
}
