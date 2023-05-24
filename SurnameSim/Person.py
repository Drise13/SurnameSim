import random
import numpy as np

male = 1
female = 2

# these reduce performance contributions from lookups by half, and because of how often they are called, any bit helps
# # $ python -m timeit -s "import numpy as np" "np.random.randint(0, 1000)"
# # 10000000 loops, best of 3: 0.168 usec per loop
#
# # $ python -m timeit -s "import numpy as np; npRandInt = np.random.randint" "npRandInt(0, 1000)"
# # 10000000 loops, best of 3: 0.0814 usec per loop
npRandInt = np.random.randint
npRandUni = np.random.uniform
npRandEx = np.random.exponential


class Person(object):
    currentName = 0
    deaths = 0
    maxAgeDistribution = []
    maxChildDistribution = [0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5]

    def __init__(self, parent=None):
        if parent is None:
            Person.currentName += 1
            self.surname = Person.currentName
        else:
            self.surname = parent.surname

        self.gender = random.choice([male, female])

        self.currentChildCount = 0
        self.currentAge = 0

        self.maxChildren = random.choice(Person.maxChildDistribution)  # cap how many children one person can have
        self.maxAge = random.choice(Person.maxAgeDistribution)

        self.childDifficulty = npRandInt(0, 100 + 1) / 100.0  # how difficult is it to have a child
        self.childYearsStart = npRandInt(10, 15 + 1)
        self.childYearsEnd = npRandInt(30, 50 + 1)

        self.partner = None  # some other person object
        self.withChild = False
        self.hasHadPartner = False  # give a way to check if they have had a partner before, so if one dies, they don't get another

        # determine age person is eligible to gain a partner. Having this too high (on average) leads to excessive population death
        if self.childYearsStart < self.maxAge:
            self.partnerEligibleAge = self.childYearsStart + int(self.childYearsStart * npRandEx(0.2))

            if self.partnerEligibleAge > self.maxAge:
                self.partnerEligibleAge = self.maxAge
        else:
            self.partnerEligibleAge = self.maxAge

    def __repr__(self):
        return 'Person(Gender: %6s, Surname: %5d, Age: %2d, MaxAge: %2d, Children: %1d, MaxChildren: %1d, Partner: %5s, PartnerEligibleAge: %2d)' % \
               ("Male" if self.isMale() else "Female", self.surname, self.currentAge, self.maxAge, self.currentChildCount, self.maxChildren,
                "True" if self.partner is not None else "False", self.partnerEligibleAge)

    def gainPartner(self, partner):  # gain a partner if both persons are eligible, female taking the surname of the other partner
        if self.isPartnerEligible():
            if self.gender == female:
                self.surname = partner.surname

            self.partner = partner
            self.hasHadPartner = True

    def age(self):
        self.currentAge += 1

        if self.currentAge >= self.maxAge:  # if they outlive their lifespan, they die
            return self.die()

        percentAge = 1.0 - ((self.maxAge - self.currentAge) / float(self.maxAge))  # get their current age as a percent of their max age

        # get a random [0, 1] with an exponential distribution.
        # Magic numbers were determined experimentally, seeing what allowed population growth, and then by checking the distribution using pyplot
        randDeathPercent = 1.0 - npRandEx(0.05) * 2.0

        # random accidents happen, so even if they haven't reached their max age,
        # there is some risk of death, especially as they get closer to their max age
        return self.die() if percentAge > randDeathPercent else False  # perform self.die, which returns True, if they have died

    def gainChild(self):  # gain a child if eligible and passes a "difficulty" check
        if self.isChildEligible():
            chance = 1.0 - (self.childDifficulty * self.partner.childDifficulty)
            if chance > npRandUni(0, 1):
                self.withChild = True

    def haveChild(self, modifier=2.0):  # produce child if they currently have one and pass a modified "difficulty" check
        if self.withChild is True:
            self.withChild = False  # pass or fail, they will no longer have a child

            chance = 1.0 - self.childDifficulty
            if chance * modifier > npRandUni(0, 1):  # if this modifier is set too low, not enough children are created and the population dies
                self.currentChildCount += 1
                return Person(parent=self)  # return a new person, giving it the last name of the parent

        return None  # if we get this far, a child fails to be created

    def die(self):
        # if they have a partner, remove themselves from that person
        if self.partner is not None:
            self.partner.partner = None
        Person.deaths += 1  # add to the death stats
        return True

    def hasPartner(self):
        return self.partner is not None

    def isChildEligible(self):  # check if person can have a child
        # Criteria:
            # only females can have children
            # they must have a partner
            # they haven't had more than their max children
            # they are within child bearing years, and so is their partner
            # they don't currently have a child
        return self.gender == female and \
               self.partner is not None and \
               self.currentChildCount < self.maxChildren and \
               self.childYearsStart <= self.currentAge <= self.childYearsEnd and \
               self.partner.childYearsStart <= self.partner.currentAge <= self.partner.childYearsEnd and \
               self.withChild is False

    def isPartnerEligible(self):  # check if person can gain a partner
        # Criteria:
            # they don't currently have a partner
            # they are old enough to have a partner
            # they haven't had a partner in the past
        return self.partner is None and \
               self.currentAge >= self.partnerEligibleAge and \
               self.hasHadPartner is False

    # helper functions to check for gender.
    # Performance profiling indicates these calls are expensive over time, so it is better to check this directly
    def isMale(self):
        return self.gender == male

    def isFemale(self):
        return self.gender == female