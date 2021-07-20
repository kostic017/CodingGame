If functions returns `any`, variants with `_i` (int), `_f` (float), and `_b` (bool) suffixes are available.

# Turret

```
int ID;
float RANGE;              // doseg topa

float x();                // x-koordianta topa
float y();                // y-koordinata topa

float robot_x(int index); // x-koordinata robota
float robot_y(int index); // y-koordinata robota
int robot_count();        // trenutan broj robota

void shoot(int index);    // markiranje trenutne mete

float sqrt(float n);
void print(any message);
```

# Robot

```
int ID;

int EXIT_C;       // kolona u grid-u u kojoj se nalazi izlaz
int EXIT_R;       // red u grid-u u kome se nalazi izlaz

int LEVEL_WIDTH;  // širina grida
int LEVEL_HEIGHT; // visina grida

int c();          // kolona u grid-u u kojoj se robot nalazi
int r();          // red u grid-u u kome se robot nalazi

void move_up();
void move_down();
void move_left();
void move_right();

string get_tile(int col, int row);

int string_length(string str);
string string_char(string str, int index);

int queue_create();
void queue_destroy(int queue);
void queue_enqueue(int queue, any value);
any queue_dequeue(int queue);
bool queue_empty(int queue);

int list_create();
void list_destroy(int list);
void list_add(int list, any value);
void list_remove(int list, int index);
any list_get(int list, int index);
int list_size(int list);

int set_create();
void set_destroy(int set);
void set_add(int set, any value);
void set_remove(int set, any value);
bool set_in(int set, any value);

void print(any message);

void global_set(string name, any value);
void global_unset(string name);
bool global_check(string name);
any global_get(string name);
```