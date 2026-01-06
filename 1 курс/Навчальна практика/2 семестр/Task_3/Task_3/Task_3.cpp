#include <iostream>

using namespace std;

struct Time {
    int hour;
    int minute;
    int second;
};

int calculateCallDuration(const Time& start, const Time& end) {
    if (start.hour < 0 || start.hour > 23 || start.minute < 0 || start.minute > 59 || start.second < 0 || start.second > 59 ||
        end.hour < 0 || end.hour > 23 || end.minute < 0 || end.minute > 59 || end.second < 0 || end.second > 59) {
        cout << "Invalid time. Hours must be from 0 to 23, minutes from 0 to 59, second from 0 to 59" << endl;
        exit(1); 
    }

    if (end.hour < start.hour || (end.hour == start.hour && end.minute < start.minute) || (end.hour == start.hour && end.minute == start.minute && end.second < start.second)) {
        cout << "Invalid time. The end time of the conversation cannot be less than the start time" << endl;
        exit(1); 
    }

    int startSeconds = start.hour * 3600 + start.minute * 60 + start.second;
    int endSeconds = end.hour * 3600 + end.minute * 60 + end.second;
    int callDurationSeconds = endSeconds - startSeconds;

    int callDurationMinutes = (callDurationSeconds + 30) / 60;

    return callDurationMinutes;
}

int main() {
    Time startTime;
    cout << "Enter start time (hours minutes seconds): ";
    cin >> startTime.hour >> startTime.minute >> startTime.second;

    Time endTime;
    cout << "Enter end time (hours minutes seconds): ";
    cin >> endTime.hour >> endTime.minute >> endTime.second;

    int callDuration = calculateCallDuration(startTime, endTime);
    cout << "Call duration: " << callDuration << " minutes" << endl;

    return 0;
}
