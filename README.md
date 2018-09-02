# cherry
integral's personal irc bot

## 개요
.NET core기반으로 작동하는 irc 채팅 봇입니다  
채널에 옵 뿌리기, 인사, 외에도 다른 여러가지 기능들을 할 수 있게 만들 것입니다  
채널마다 스트림이 있고, 이 스트림의 이벤트를 기반으로 작동하므로 기능의 on/off가 쉽습니다  
~~델리게이트 쥑이네~~
## TODO
  * [X] TCP 소켓 통신 구현
  * [X] IRC 패킷의 parsing 구현(부분적 완료)
  * [X] 스레드 방식의 I/O 구현
  * [ ] 가능한 모든 command 입력에 따른 파싱 구현
  * [X] Service 구조 레이아웃 잡기
  * [X] Network 구조 리팩토링
