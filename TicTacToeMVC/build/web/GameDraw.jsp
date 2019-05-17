<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@page import="ModelClasses.*"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    </head>
    <body>
        <%
            Game game=(Game)session.getAttribute("game");
            int n=game.getSize();
            
        %>
        <table border="1" width="300" height="300" >
            <%for(int i=0;i<n;i++){%>
                <tr>
                    <%for(int j=0;j<n;j++){%>
                        <td>
                            <input type="text" name="<%=game.getCellName(i,j)%>"
                                   value="<%=game.getCellValue(i,j)%>"
                                   size="1" maxlength="1" 
                                   <%if(!game.getCellValue(i, j).isEmpty()){%>
                                   readonly
                                   <%}%>
                                   style="text-align:center; width:100%; height:100%; 
                                   <%if(game.isStrongCell(i,j)){%> font-weight:bold;<%}%>
                                   "
                            >
                         </td>
                    <%}%>
                </tr>
            <%}%>    
        </table>
    </body>
</html>
