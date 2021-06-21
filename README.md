If functions returns `any`, variants with `_i` (int), `_f` (float), and `_b` (bool) suffixes are available.

# Turret

```
int ID;
float RANGE;

float x;
float y;

float robot_x(int index);
float robot_y(int index);
int robot_count();

void shoot(int index);

float sqrt(float n);
void print(any message);
```

# Robot

```
int r;
int c;

int ID;

int EXIT_C;
int EXIT_R;

int LEVEL_WIDTH;
int LEVEL_HEIGHT;

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