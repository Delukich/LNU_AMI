#include "pch.h"
#include <gtest/gtest.h>
#include "C:/Алгоритми та структури даних/Lab_3/Lab_3/Lab_3.cpp"  

TEST(CountryTest, DisplayInfo) {
    Country country("TestCountry", "TestCapital", 100.0, 1000000);

    testing::internal::CaptureStdout();
    country.print();
    std::string output = testing::internal::GetCapturedStdout();

    ASSERT_TRUE(output.find("TestCountry") != std::string::npos);
    ASSERT_TRUE(output.find("TestCapital") != std::string::npos);
    ASSERT_TRUE(output.find("100.0") != std::string::npos);
    ASSERT_TRUE(output.find("1000000") != std::string::npos);
}

int main(int argc, char** argv) {
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}
