ROBOT

```
EXIT_X
EXIT_Y
LEVEL_WIDTH
LEVEL_HEIGHT

int x();
int y();

void move_up();
void move_down();
void move_left();
void move_right();

string get_tile(int x, int y);

int string_length(string str);
string string_char(string str, int idx);

int queue_create();
void queue_destroy(int queue);
void queue_enqueue(int queue, any value);
any queue_dequeue(int queue);
bool queue_empty(int queue);

int list_create();
void list_destroy(int list);
void list_add(int list, any value);
void list_remove(int list, int idx);
any list_get(int list, int idx);
int list_size(int list);

void set_create();
void set_destroy(int set);
void set_add(int set, any value);
void set_remove(int set, any value);
bool set_contains(int set, any value);

void print(string value);
```