open Graphics;; 
open Thread;;

exception EmptyQueue;;
(*---------- Type Define ----------*)
type 'a queue = 'a list;;
type t_button = {id:int; color:Graphics.color; color1:Graphics.color;
        sound:int; mutable arc_s:int; mutable arc_d:int; size:int};;
type t_buttons = t_button list;;
type t_status = {mutable stage:int; mutable life:int;
             mutable wait:bool; mutable game_over:bool; mutable help:bool; mutable time:int; mutable menu:bool};; 

(*---------- Create window -------------*)
let frame =
 Graphics.open_graph " 400x400";
 Graphics.set_window_title "MEMORIZE GAME by 'a_ram";;
let get_x = Graphics.size_x frame and get_y = Graphics.size_y frame;; 
(*---------- Variable Initialize ----------*)
let status = {stage=1; life=3; wait=true; game_over=false; help=false; time=250; menu=true};;
let bgcolor = Graphics.black;;
let buttons = [{id=0; color=0xAA0000; color1=0xFF0000; sound=261; arc_s=0; arc_d=90; size=80};
        {id=1; color=0x0000AA; color1=0x0000FF; sound=293; arc_s=90; arc_d=180; size=80};
        {id=2; color=0x00AA00; color1=0x00FF00; sound=329; arc_s=180; arc_d=270; size=80};
        {id=3; color=0xAAAA00; color1=0xFFFF00; sound=349; arc_s=270; arc_d=360; size=80};
        {id=4; color=0xAA00AA; color1=0xFF00FF; sound=39; arc_s=0; arc_d=360; size=30}];;
let rank = ref [0;0;0];;
let empty = ref [];;
let queue = ref ([]:int list);; 
let reset_state () =
 status.stage <- 1;
 status.life <- 3;
 status.wait <- true;
 status.game_over <- false;
 status.help <- false;
 status.time <- 250;; 

(*---------- Common Functions ----------*)
let get_btn_color n = (List.nth buttons n).color;;
let get_btn_color1 n = (List.nth buttons n).color1;;
let play_sound n = Graphics.sound (List.nth buttons n).sound 200;; 
let judge_button x y =
 let color = Graphics.point_color x y
 in
 match color with
 | c when c = (List.nth buttons 0).color -> 0
 | c when c = (List.nth buttons 1).color -> 1
 | c when c = (List.nth buttons 2).color -> 2
 | c when c = (List.nth buttons 3).color -> 3
 | c when c = (List.nth buttons 4).color -> 4
 | _ -> -1;; 

let rec qsort list = match list with
 | [] -> []
 | pivot::rest ->
  let left, right = List.partition (function x -> x>pivot) rest
   in
   qsort left @ [pivot] @ qsort right;; 
(*---------- Draw Functions ----------*)
let draw_button n t =
 let size =
  (List.nth buttons n).size
 and arc_s =
  (List.nth buttons n).arc_s
 and arc_d =
  (List.nth buttons n).arc_d
 in
 if n=t then (
  Graphics.set_color (get_btn_color1 n)
 ) else (
  Graphics.set_color (get_btn_color n)
 );
 Graphics.fill_arc (get_x/2) (get_y/2) size size arc_s arc_d;; 

let draw_clicked_button n =
 Graphics.set_color bgcolor;
 Graphics.fill_rect (get_x/2-80) (get_y/2-80) 160 160;
 draw_button 0 n;
 draw_button 1 n;
 draw_button 2 n;
 draw_button 3 n;
 Graphics.set_color bgcolor;
 Graphics.fill_rect ((get_x/2)-80) ((get_y/2)-5) 160 10;
 Graphics.fill_rect ((get_x/2)-5) ((get_y/2)-80) 10 160;
 Graphics.fill_circle (get_x/2) (get_y/2) 40;
 draw_button 4 n;; 
 
let init_draw () =
 let n = (-1)
 in
 Graphics.clear_graph frame;
 Graphics.set_color bgcolor;
 Graphics.fill_rect 0 0 get_x get_y;
 draw_clicked_button n;; 

let draw_stage () =
 if not status.menu then (
  Graphics.moveto 10 (get_y-20);
  Graphics.set_color Graphics.white;
  Graphics.draw_string ("Stage: " ^ string_of_int status.stage);
 );; 
 
let draw_life () =
 if not status.menu then (
  Graphics.moveto 150 (get_y-20);
  Graphics.set_color Graphics.white;
  Graphics.draw_string ("Life: " ^ string_of_int status.life);
 );; 

let draw_title () =
 if status.menu then (
  Graphics.set_color 0x0066ff;
  for i = 0 to 9 do
   Graphics.fill_circle (get_x/2-65+(15*i)) (get_y-45) 15;
  done;
  Graphics.set_color 0xffffff;
  Graphics.moveto (get_x/2-70) (get_y-51);
  Graphics.draw_string ("MEMORIZE GAME v1.0")
 );; 
 
let draw_wait () =
 if not status.menu then (
   if status.wait then (
   Graphics.set_color 0xCC0033;
   Graphics.fill_circle 17 (get_y-45) 7;
   Graphics.fill_circle 174 (get_y-45) 7;
   Graphics.fill_rect 15 (get_y-52) 160 15;
   Graphics.moveto 15 (get_y-50);
   Graphics.set_color 0x00FFFF;
   Graphics.draw_string ("!!KEEP YOUR MEMORY!!")
  ) else (
   Graphics.set_color 0x00FFFF;
   Graphics.fill_circle 17 (get_y-45) 7;
   Graphics.fill_circle 184 (get_y-45) 7;
   Graphics.fill_rect 15 (get_y-52) 170 15;
   Graphics.moveto 15 (get_y-50);
   Graphics.set_color 0x800000;
   Graphics.draw_string ("!!CLICK THE BUTTONS!!")
  );
 );; 

let draw_menu () =
 if status.menu then (
  Graphics.set_color Graphics.white;
  Graphics.moveto 10 20;
  Graphics.draw_string "1.Start  2.How to play  3.Rank";
 );
 Graphics.moveto 250 20;
 Graphics.set_color Graphics.red;
 Graphics.draw_string "  ESC. EXIT";;
  
let draw_gameover () =
 if(status.game_over) then (
  Graphics.set_color Graphics.white;
  for i = 0 to 2 do
   Graphics.draw_rect (get_x/2 - 70+i) (get_y/2-13+i) 140 28;
  done;
  Graphics.moveto (get_x/2 - 60) (get_y/2 - 4);
  Graphics.draw_string "G A M E O V E R";
  rank := (status.stage :: !rank)
 );; 

let draw_help () =
 if(status.help) then (
  Graphics.set_color 0x4169E1;
  Graphics.fill_rect 45 35 (get_x - 80) (get_y - 70);
  Graphics.set_color 0x00BFFF;
  Graphics.fill_rect 40 40 (get_x - 80) (get_y - 70);
  Graphics.set_color 0x4B0082;
  Graphics.fill_rect 45 (get_y - 65) 302 2;
  Graphics.set_color Graphics.black;
  Graphics.moveto 155 320; Graphics.draw_string "## MANUAL ##";
  Graphics.moveto 50 270; Graphics.draw_string "1.Memory the order of the pressed";
  Graphics.moveto 50 255; Graphics.draw_string "  buttons of wait state.";
  Graphics.moveto 50 235; Graphics.draw_string "2.Press the buttons in order.";
  Graphics.moveto 50 215; Graphics.draw_string "3.If incorrect button is pressed,";
  Graphics.moveto 50 200; Graphics.draw_string "  life is decreased.";
  Graphics.moveto 50 180; Graphics.draw_string "4.If you clear the stage when";
  Graphics.moveto 50 165; Graphics.draw_string "  time bar is green, then +1 life.";
  Graphics.moveto 50 50; Graphics.draw_string "Press 'ENTER' to start game";
 );; 

let draw_rank () =
 Graphics.set_color 0x666666;
 Graphics.fill_rect (get_x/2-35) (get_y/2-45) 80 80;
 Graphics.set_color 0xffffcc;
 Graphics.fill_rect (get_x/2-38) (get_y/2-42) 80 80;
 Graphics.set_color 0x4B0082;
 Graphics.moveto (get_x/2-29) (get_y/2+18);
 Graphics.draw_string ("R A N K");
 Graphics.draw_rect (get_x/2-34) (get_y/2+13) 68 1;
 rank := qsort !rank;
 Graphics.set_color 0x000000;
 Graphics.moveto (get_x/2-28) (get_y/2-5);
 Graphics.draw_string ("1st : " ^ (string_of_int (List.nth !rank 0)));
 Graphics.moveto (get_x/2-28) (get_y/2-20);
 Graphics.draw_string ("2nd : " ^ (string_of_int (List.nth !rank 1)));
 Graphics.moveto (get_x/2-28) (get_y/2-35);
 Graphics.draw_string ("3rd : " ^ (string_of_int (List.nth !rank 2)));;
 
 
let draw_time () =
 if status.time > (250 - (status.stage * 8)) then (
  Graphics.set_color Graphics.green;
 ) else (
  Graphics.set_color Graphics.red;
 );
 Graphics.fill_rect 310 50 25 250;
 Graphics.set_color Graphics.white;
 Graphics.fill_rect 310 (50+status.time) 25 (250-status.time);; 
 
let repaint () =
 init_draw ();
 draw_stage ();
 draw_life ();
 draw_wait ();
 draw_menu ();
 draw_gameover ();
 draw_help ();
 draw_time();
 draw_title();; 

let paint () = repaint ();; 

(*---------- Event Dealing Function -----------*)
let click_effect n =
 if 0<=n && n<=4 then (
  draw_clicked_button n;
  play_sound n;
  draw_clicked_button (-1)
 );; 

let rec flush () =
 if Graphics.key_pressed () then (
  let x = Graphics.read_key () in flush (); ignore x;
 ) else if Graphics.button_down () then (
  let y = Graphics.mouse_pos in flush()
 );; 

(*---------- random_create Queue ----------*)
let dequeue q = match !q with
 | [] -> raise EmptyQueue
 | h::t ->
 q := t;
 h;; 
 
let rec random_create que = function
 | 0 -> ()
 | x -> let rand =
    let t = Sys.time () in
    let n = int_of_float (t*.1000.0)
    in
    Random.init (n mod 100000);
    Random.int 5
    in
    click_effect rand;
    Graphics.sound 10000 100;
    que := !que @ [rand];
    random_create que (x-1);;  
(*---------- Check user input ----------*)
let is_correct key =
 if key <> (-1) then (
  if (key <> (dequeue queue)) then (
   status.life <- (status.life -1);
   status.wait <- true;
   queue := !empty;
   if (status.life <= 0) then (
    status.wait <- true;
    status.game_over <- true
   )
  ) else (
   if (!queue = !empty) then (
    if status.time > (250 - (status.stage * 8)) then (
     status.life <- (status.life + 1)
     );
    status.stage <- (status.stage +1);
    status.wait <- true
   )
  )
 );; 
(*---------- Time Progress Bar Threads -----------*)
let decrease () =
 while not status.game_over do ( 
  if status.wait then (
   status.time <- 250;
   draw_time();
   Thread.delay (0.2 *. float_of_int(status.stage) +. 0.5);
  ) else (
   status.time <- (status.time - 1);
   Thread.delay 0.01;
   draw_time();
  );
  if status.time = 0 then (
   status.life <- (status.life - 1);
   if (status.life = 0) then (
    status.wait <- true;
    status.game_over <- true;
    status.menu <- true;
   );
   status.time <- 250;
   repaint()
  )
 ) done;
 Thread.kill;; 
  
(*---------- Main Game Thread Threads -----------*)
let start_game () =
 let td_time = Thread.create decrease()
 in
 while not(status.game_over) do (
  if status.life = 0 then (
   status.wait <- false;
   status.game_over <- true
  );
  while status.wait do ( (* Don't modify this block!!!!! *)
   Thread.delay 1.0;
   status.wait<-false;
   random_create queue status.stage;
   draw_wait ();
   flush()
  ) done;  
  let st = ref (Graphics.wait_next_event [Graphics.Poll])
  in
  if (!st.Graphics.keypressed || !st.Graphics.button) then (
   st := (Graphics.wait_next_event [Graphics.Button_down;Graphics.Key_pressed]);
   if not(status.wait) then (
    if !st.Graphics.button then (
     let (x, y) = Graphics.mouse_pos ()
     in
     let bt_number = judge_button x y
     in
     if bt_number <> -1 then (
      click_effect bt_number;
      is_correct bt_number;
      repaint ()
     ) else (
      flush()
     )
    ) else if !st.Graphics.keypressed then (
     let key = !st.Graphics.key
     in
     (
      match (int_of_char(key)) with
      | 49 -> click_effect 2; is_correct 2; repaint ()
      | 51 -> click_effect 3; is_correct 3; repaint ()
      | 53 -> click_effect 4; is_correct 4; repaint ()
      | 55 -> click_effect 1; is_correct 1; repaint ()
      | 57 -> click_effect 0; is_correct 0; repaint ()
      | 27 -> close_graph ()
      | _ -> ();
     )
    )
   )
  )
  ) done;; 
(*---------- Menu ----------*)
let rec help_menu () =
 init_draw ();
 draw_menu ();
 draw_help ();
 let st = Graphics.wait_next_event [Graphics.Key_pressed]
 in
 if st.Graphics.keypressed then (
  let key = int_of_char(st.Graphics.key)
  in
  match key with
  | 13 | 27 -> status.help <- false; status.menu <- false; paint (); start_game ();
  | _ -> help_menu()
 );; 

let start () =
 while true do (
  flush();
  status.menu <- true;
  paint();
  let sta = Graphics.wait_next_event [Graphics.Key_pressed]
  in
  if sta.Graphics.keypressed then (
   let key = int_of_char(sta.Graphics.key)
   in
   match key with
   | 49 | 13 -> status.menu <- false; repaint(); start_game (); Thread.delay 3.0 (* Don't remove this delay *)
   | 50 -> status.help <- true; help_menu()
   | 51 -> draw_rank (); Thread.delay 3.0 (* Don't remove this delay *)
   | 27 -> close_graph ()
   | _ -> ()
  );
  reset_state ();
 ) done;; 

(*---------- MAIN FUNCTION ----------*)
start();  