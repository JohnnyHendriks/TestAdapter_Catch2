/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

#pragma once
#ifndef _Catch2_CATCH_HPP
#define _Catch2_CATCH_HPP

#ifdef TA_CATCH2_V2_0_1

#include "v2.0.1/catch.hpp"

#elif defined TA_CATCH2_V2_1_0

#include "v2.1.0/catch.hpp"

#elif defined TA_CATCH2_V2_1_1

#include "v2.1.1/catch.hpp"

#elif defined TA_CATCH2_V2_1_2

#include "v2.1.2/catch.hpp"

#elif defined TA_CATCH2_V2_2_0

#include "v2.2.0/catch.hpp"

#elif defined TA_CATCH2_V2_2_1

#include "v2.2.1/catch.hpp"

#elif defined TA_CATCH2_V2_2_2

#include "v2.2.2/catch.hpp"

#elif defined TA_CATCH2_V2_2_3

#include "v2.2.3/catch.hpp"

#elif defined TA_CATCH2_V2_3_0

#include "v2.3.0/catch.hpp"

#elif defined TA_CATCH2_V2_4_0

#include "v2.4.0/catch.hpp"

#elif defined TA_CATCH2_V2_4_1

#include "v2.4.1/catch.hpp"

#elif defined TA_CATCH2_V2_4_2

#include "v2.4.2/catch.hpp"

#elif defined TA_CATCH2_V2_5_0

#include "v2.5.0/catch.hpp"

#elif defined TA_CATCH2_V2_6_0

#include "v2.6.0/catch.hpp"

#elif defined TA_CATCH2_V2_6_1

#include "v2.6.1/catch.hpp"

#elif defined TA_CATCH2_V2_7_0

#include "v2.7.0/catch.hpp"

#else

#include "v2.7.0/catch.hpp"

#endif

#endif // End of _Catch2_CATCH_HPP definition
