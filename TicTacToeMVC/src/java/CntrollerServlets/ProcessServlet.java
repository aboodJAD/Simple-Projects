
package CntrollerServlets;

import ModelClasses.*;
import java.io.*;
import java.util.*;
import java.util.Map.*;
import javax.servlet.*;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.*;
@WebServlet(name = "ProcessServlet", urlPatterns = {"/ProcessServlet"})
public class ProcessServlet extends HttpServlet {

    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        HttpSession session=request.getSession();
        try (PrintWriter out = response.getWriter()) {
            Game game=(Game)session.getAttribute("game");
            Map<String,String > updateList=new HashMap<String,String>();
            Enumeration paramNames=request.getParameterNames();
            while(paramNames.hasMoreElements()){
                String key=(String)paramNames.nextElement();
                String value=request.getParameter(key);
                if("Cell".equals(key.substring(0, 4))){
                    updateList.put(key,value);
                }
            }
            try{
                game.updateGame(updateList);
            }catch(IllegalArgumentException e){
                request.setAttribute("errormessage",e.getMessage());
                RequestDispatcher rq=request.getRequestDispatcher("InGame.jsp");
                rq.forward(request,response);
                return ;
            }
            session.setAttribute("game", game);
            if(game.isEnd()){
                RequestDispatcher rq=request.getRequestDispatcher("GameEnd.jsp");
                rq.forward(request,response);
            }else{
                RequestDispatcher rq=request.getRequestDispatcher("InGame.jsp");
                rq.forward(request,response);
            }
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
